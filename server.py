from flask import Flask, request

app = Flask(__name__)

@app.route('/', methods=['POST'])
def handle_post_request():
    message = request.get_data(as_text=True)

    print("Received message:", message)

    # Process the message here
    # Replace the following lines with your desired logic

    if message:
        response = "A Message received: " + message
    else:
        response = "No message received."

    return response

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
