import whisper
import tempfile
import requests
from flask import Flask, request, jsonify

app = Flask(__name__)
model = whisper.load_model("base")  # Load Whisper model locally

TRANSLATION_API_URL = "http://127.0.0.1:5001/translate"  # URL of backend_reality.py

@app.route('/capture_speech', methods=['POST'])
def capture_speech():
    if "audio" not in request.files:
        return jsonify({"error": "No audio file received"}), 400

    audio_file = request.files["audio"]

    # Save audio to a temporary file
    with tempfile.NamedTemporaryFile(delete=False, suffix=".wav") as temp_audio:
        audio_file.save(temp_audio.name)
        result = model.transcribe(temp_audio.name)

    transcribed_text = result["text"]
    print(f"✅ Transcribed: {transcribed_text}")

    # Send transcribed text to translation API
    translation_response = requests.post(
        TRANSLATION_API_URL,
        json={"from_language": "es", "to_language": "en", "user_input": transcribed_text}
    )

    if translation_response.status_code == 200:
        translated_text = translation_response.json()["translated_text"]
        print(f"✅ Translated: {translated_text}")
        return jsonify({"success": True, "transcribed_text": transcribed_text, "translated_text": translated_text})
    else:
        return jsonify({"success": False, "error": "Translation failed"}), 500

if __name__ == "__main__":
    app.run(debug=True, port=5002)
