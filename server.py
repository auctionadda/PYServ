from flask import Flask, request
import base64

app = Flask(__name__)

@app.route('/', methods=['POST'])
def handle_post_request():
    base64_data = request.get_data(as_text=True)

    print("Received base64 data:", base64_data)
    
    # Process the base64 data here
    # For example, you can save it to a file, perform some computation, etc.
    # Replace the following lines with your desired logic
    
    if base64_data:
        # Check if padding is needed and add it if necessary
        padding_length = 4 - (len(base64_data) % 4)
        base64_data += '=' * padding_length
        
        # Decode the base64 data
        try:
            decoded_data = base64.b64decode(base64_data)
            
            # Save the decoded data to a file
            filename = f"RecivedData/data.txt"
            with open(filename, "wb") as file:
                file.write(decoded_data)

            print("File saved to:", filename)
            
            response = "Data received and saved to file: " + filename
        except base64.binascii.Error as e:
            response = "Error decoding base64 data: " + str(e)
    else:
        response = "No data received."
    
    return response

if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)
