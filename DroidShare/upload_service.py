from flask import Flask, request
import os

app = Flask(__name__)
UPLOAD_FOLDER = 'photos_default'
os.makedirs(UPLOAD_FOLDER, exist_ok=True)

@app.route('/upload', methods=['POST'])
def upload_file():
    if 'file' not in request.files:
        return 'No file part', 400
    file = request.files['file']
    if file.filename == '':
        return 'No selected file', 400

    # Guardar el archivo en la carpeta de subidas
    file_path = os.path.join(UPLOAD_FOLDER, file.filename)
    file.save(file_path)
    return 'File successfully uploaded', 200

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
