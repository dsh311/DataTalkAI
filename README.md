# DataTalkAI

DataTalkAI is a WPF-based desktop application that allows users to load and interact with local Large Language Models (LLMs) using GGUF format models. It provides a simple chat-style interface for sending prompts and receiving model completions.

---

## 🚀 Features

- Local LLM inference (no cloud required)
- Chat-style UI built with WPF
- Support for GGUF models
- Fast CPU-based inference via LLamaSharp backend

---

## 📦 Requirements

To use DataTalkAI, you must download a compatible GGUF model file.

### Recommended Model

We recommend using:

**mistral-7b-instruct-v0.2.Q4_K_M-GGUF**

Download here:  
https://huggingface.co/jonahhenry/mistral-7b-instruct-v0.2.Q4_K_M-GGUF/tree/main

Place the downloaded `.gguf` file in a known directory and load it from within the application.

---

## 🧠 How It Works

DataTalkAI uses LLamaSharp to run inference locally on CPU (or GPU if configured via backend). The application loads a GGUF model and allows you to send prompts through a simple chat interface.

---

## 📚 Dependencies

This project uses the following NuGet packages:

### 🔹 LlamaSharp
- **License:** MIT  
- **Authors:** Rinne, Martin Evans, jlsantiago, and contributors  
- **Repository:** https://github.com/SciSharp/LLamaSharp  

### 🔹 LLamaSharp.Backend.Cpu
- **License:** MIT  
- **Authors:** llama.cpp authors  
- **Repository:** https://github.com/SciSharp/LLamaSharp  

---

## 🛠️ Setup Instructions

1. Clone the repository
2. Restore NuGet packages
3. Download a compatible GGUF model (see above)
4. Run the application
5. Load the model inside the UI
6. Start chatting!

---

## ⚠️ Notes

- Large models (like 7B) require sufficient RAM (8GB+ recommended)
- CPU inference is slower than GPU but fully functional
- Ensure the model file matches the GGUF format supported by LLamaSharp

---

## 📄 License

This project is provided as-is for educational and experimental purposes.

LLamaSharp and its backend are licensed under MIT.
