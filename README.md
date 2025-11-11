# ST2-2025-Team-17-SmartGrocery

## Author
Maria-Magdalena Delcheva, Stefani Stefanova, Yoana Andreeva

## Overview
This project demonstrates a Smart Grocery MVC application enhanced with an AI Recipe Assistant powered by a local LLM (Orca-Mini / Mistral via GPT4All). The system analyzes the user's shopping list and automatically generates short recipe suggestions based on the available products. It runs fully locally and does not require an internet connection.

### Key Features
- Create and manage shopping lists  
- Send list contents to local AI for recipe generation  
- Offline, privacy-friendly inference using GPT4All  
- Integration between **ASP.NET Core MVC** and **FastAPI (Python)**  

---

## Project Structure

| Directory | Description |
|------------|-------------|
| `SmartGrocery/SmartGrocery/` | Main ASP.NET Core MVC application |
| `SmartGrocery/SmartGrocery/Controllers` | Contains MVC controllers (e.g., `AiController.cs`) |
| `SmartGrocery/SmartGrocery/Services` | Contains `AiService.cs` – handles connection to Python AI server |
| `SmartGrocery/SmartGrocery/Views` | Razor views for shopping lists and AI recipes |
| `pythonServer/` | FastAPI server using GPT4All |
| `pythonServer/ai_server.py` | Core AI API script that loads local model and answers recipe requests |
| `pythonServer/models/` | Folder for downloaded `.gguf` model files (e.g., Orca-mini or Mistral) |


---

## Requirements
- .NET 8 SDK  
- Python 3.10 – 3.12  
- Required Python libraries: pip install gpt4all fastapi uvicorn pydantic
- Download a .gguf model compatible with GPT4All and place it inside:

pythonServer/models/

Example:
orca-mini-3b-gguf2-q4_0.gguf


---

## Start Instructions
### 1) Run the Python AI server
-Navigate to the Python folder:
cd pythonServer

- Make sure the model exists at:
pythonServer/models/orca-mini-3b-gguf2-q4_0.gguf

- Then start the server:
uvicorn ai_server:app --host 127.0.0.1 --port 5000

### 2) Start MVC App
- From your main solution folder:

cd SmartGrocery/SmartGrocery
dotnet restore
dotnet run


- Check `appsettings.json` → LLM section must point to:
```
http://127.0.0.1:5000
```
## Used Design Patterns
Used Design Patterns
Pattern	Where	Purpose
MVC	ASP.NET MVC	Separation of UI / Logic / Data
Repository Pattern	DatabaseContext.cs	Encapsulates DB logic
Singleton	AI model instance	Ensures only one local model is loaded
Dependency Injection	AiService, UserManager	Loose coupling and testability
Options Pattern	appsettings.json	Configurable AI endpoint
Facade	AiService	Simplifies interaction with Python API
DTO	SuggestRequest, Query	Structured data between systems
Validation Pattern	DataAnnotations	Safe input validation

## Troubleshooting

| Problem | Cause | Fix |
|----------|--------|-----|
| `UnicodeDecodeError` when running `ai_server.py` | File encoding not UTF-8 | Re-save the file in UTF-8 (e.g., in VS Code → Save with Encoding → UTF-8) |
| `Request failed: HTTP 404 Not Found` | GPT4All couldn’t download the model | Manually download the `.gguf` file and place it in `pythonServer/models/` |
| `bind: Only one usage of each socket address` | Port 11434 already in use by another Ollama or GPT4All process | Kill the process using `taskkill /PID <pid> /F` and restart |
| ASP.NET app shows “No instruction found” | AI server returned empty or malformed response | Adjust prompt formatting and check FastAPI console for errors |
| MVC app can’t reach AI server | Wrong URL or port | Ensure both use the same port (`127.0.0.1:5000`) and check `appsettings.json` BaseUrl |
| Slow model loading | Using large model (e.g. 7B+) | Switch to a smaller quantized model like `orca-mini-3b-gguf2-q4_0.gguf` |
| `UnicodeDecodeError` or `SyntaxError` during run | Non-UTF characters in comments or docstrings | Remove non-ASCII characters or specify encoding at top: `# -*- coding: utf-8 -*-` |
| AI not responding | Model path incorrect | Verify `model_path` in `ai_server.py` matches your real folder (e.g. `D:\\GPT4All_Models`) |
| “Connection refused” error in MVC | FastAPI not running | Start Python server with `uvicorn ai_server:app --host 127.0.0.1 --port 5000` |
| AI returns weird symbols | Model file corrupted or incomplete | Delete and re-download `.gguf` model |
| Bulgarian output sounds unnatural | Model not trained for BG language | Write prompts in English or mix short BG + English words |

