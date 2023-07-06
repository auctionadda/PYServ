from flask import Flask, request
import base64
import os
import urllib.parse

app = Flask(__name__)

# Counter for assigning unique IDs
id_counter = 0

@app.route('/', methods=['POST'])
def handle_post_request():
    global id_counter

    # Generate a unique ID for the client
    client_id = str(id_counter)
    id_counter += 1

    # Get the base64 string from the request
    message = request.form.get('base64_data')
    message = urllib.parse.unquote(message)
    print("Received message encoded:", message)

    # Remove the "data:image/png;base64," prefix
    prefix = "data:image/png;base64,"
    if message.startswith(prefix):
        message = message[len(prefix):]

    # Save the base64 string as an image
    try:
        # Create the inputimages folder if it doesn't exist
        if not os.path.exists('inputimages'):
            os.makedirs('inputimages')

        # Save the image with a unique filename
        filename = f"inputimages/image_{client_id}.png"
        with open(filename, 'wb') as image_file:
            image_file.write(base64.b64decode(message))

        response = "Message received. Image saved as: " + filename
    except base64.binascii.Error as e:
        response = "Error decoding base64 string: " + str(e)

    return response

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
