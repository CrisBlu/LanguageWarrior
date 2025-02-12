from openai import OpenAI
import json
from dotenv import load_dotenv
from flask import Flask, request, jsonify
from flask_cors import CORS

# from flask_socketio import SocketIO, emit
import os
import string

app = Flask(__name__)
CORS(app, resources={r"/translate": {"origins": "*"}})

load_dotenv()

openai_api_key = os.getenv("API_KEY")
openai_api_base = "https://api.lambdalabs.com/v1"

client = OpenAI(
    api_key=openai_api_key,
    base_url=openai_api_base,
)

@app.route('/')
def heldo():
    return "Hello World"

@app.route('/translate', methods=['POST'])
def translate():
    data = request.json
    from_language = data.get("from_language")
    to_language = "Spanish"
    user_input = data.get("user_input")

    
    model = "llama3.3-70b-instruct-fp8"
    
    chat_completion = client.chat.completions.create(
        messages=[{
            "role": "system",
            "content": f"You are a translator that translates {from_language} to {to_language} and whatever " +
                       f"I type, you will translate into {to_language}. Do not respond to questions, provide notes, " +
                       f"or give instructions. Just translate into {to_language} the whole sentence and that's it, no matter if it's a question. " +
                       "Do not prepend anything like: 'translation to ...'. Do not specify anything else other than the translation; just translate into " +
                       f"{to_language}."
        }, {
            "role": "user",
            "content": user_input
        }],
        model=model,
    )
    
    # Get the translated text
    translation_result = chat_completion.choices[0].message.content
    print(translation_result)
    
    
    # Create a dictionary to hold the response
    response = {
        "from_language": from_language,
        "to_language": to_language,
        "user_input": user_input,
        "translated_text": translation_result,
    }
    
    print(response)
    
    # Save the result to a JSON file (optional)
    with open('translation_result.json', 'w') as json_file:
        json.dump(response, json_file, indent=4)

    # Return the translation as a JSON response
    return jsonify(response)

if __name__ == "__main__":
    app.run(debug=True, port=5001)
