from fastapi import FastAPI
from pydantic import BaseModel
from gpt4all import GPT4All
import uvicorn

app = FastAPI(title="Smart Grocery AI")

MODEL_NAME = "orca-mini-3b-gguf2-q4_0.gguf"
model = GPT4All(MODEL_NAME, model_path="D:\\GPT4All_Models")

class Query(BaseModel):
    prompt: str

@app.post("/ask")
def ask_ai(query: Query):
    """Returns answer from the AI model"""
    try:
        with model.chat_session():
            response = model.generate(query.prompt, max_tokens=100, temp=0.7)  # Increased for full output
        print(f"AI Response: {response}")  # Debug: Print to console
        return {"response": response}
    except Exception as e:
        return {"response": f"Error: {str(e)}"}

if __name__ == "__main__":
    uvicorn.run(app, host="127.0.0.1", port=5000)