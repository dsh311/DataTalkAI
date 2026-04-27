using LLama;
using LLama.Common;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace DataTalkAI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // Class-level fields to persist the AI state
        private LLamaWeights _weights;
        private LLamaContext _context;
        private ChatSession _session;
        private bool _isModelLoaded = false;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the model when the window is ready
            this.Loaded += MainWindow_Loaded;
        }


        public static string PromptForGgufModel(Window owner = null)
        {
            var dialog = new OpenFileDialog
            {
                Title = "Select LLM Model",
                Filter = "GGUF Model Files (*.gguf)|*.gguf|All Files (*.*)|*.*",
                DefaultExt = ".gguf",
                CheckFileExists = true,
                CheckPathExists = true,
                Multiselect = false
            };

            bool? result = owner != null
                ? dialog.ShowDialog(owner)
                : dialog.ShowDialog();

            if (result == true)
            {
                return dialog.FileName;
            }

            return null; // user cancelled
        }


        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // UI thread: show dialog
            string modelPath = PromptForGgufModel(this);

            if (string.IsNullOrWhiteSpace(modelPath))
            {
                MessageBox.Show("No model selected.");
                return;
            }

            CompletionTextBox.Text = $"Initializing model: {modelPath} \r\n (Loading Weights)...";

            if (!File.Exists(modelPath))
            {
                CompletionTextBox.Text = "File missing at: " + modelPath;
                return;
            }


            await Task.Run(() =>
            {
                try
                {
                    var parameters = new ModelParams(modelPath)
                    {
                        ContextSize = 4096,
                        GpuLayerCount = 0
                    };

                    // Load once and keep in memory
                    _weights = LLamaWeights.LoadFromFile(parameters);
                    _context = _weights.CreateContext(parameters);
                    var executor = new InteractiveExecutor(_context);
                    _session = new ChatSession(executor);
                    _isModelLoaded = true;
                }
                catch (Exception ex)
                {
                    Dispatcher.Invoke(() => CompletionTextBox.Text = ex.ToString());
                }
            });

            if (_isModelLoaded)
            {
                CompletionTextBox.Text = "Model is ready. Enter a prompt and click Chat.";
            }

        }



        private async void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            if (!_isModelLoaded) return;

            string userPrompt = PromptTextBox.Text;
            if (string.IsNullOrWhiteSpace(userPrompt)) return;

            PromptTextBox.Clear();
            CompletionTextBox.Clear();

            // Use the existing persistent _session
            var history = new ChatHistory.Message(AuthorRole.User, userPrompt);
            var inferenceParams = new InferenceParams { MaxTokens = 512, AntiPrompts = new[] { "User:" } };
            
            await foreach (var text in _session.ChatAsync(history, inferenceParams))
            {
                CompletionTextBox.AppendText(text);
                CompletionTextBox.ScrollToEnd();
            }
            
        }


        // Clean up memory when the app closes
        protected override void OnClosed(EventArgs e)
        {
            _context?.Dispose();
            _weights?.Dispose();
            base.OnClosed(e);
        }

        private void PromptTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ChatButton_Click(sender, e);
                e.Handled = true;
            }
        }
    }
}