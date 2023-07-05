from flask import Flask, request

app = Flask(__name__)

@app.route('/', methods=['POST'])
def handle_post_request():
    base64_data = request.get_data(as_text=True)
    
    # Process the base64 data here
    # For example, you can save it to a file, perform some computation, etc.
    # Replace the following lines with your desired logic
    
    if base64_data:
        response = "Data received and processed successfully."
    else:
        response = "No data received."
    
    return response

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000)
