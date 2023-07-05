import time
import zmq
import uuid
import base64
import os

context = zmq.Context()
socket = context.socket(zmq.REP)
socket.bind("tcp://*:5555")

clients = {}  # Dictionary to store client identities

while True:
    # Wait for next request from client
    base64_data = socket.recv()
    print("Received request: base64_data")

    # Check if the client already has an assigned ID
    client_id = clients.get(socket)

    if client_id is None:
        # Generate a new unique ID for the client
        client_id = str(uuid.uuid4())
        # Store the client's socket and its assigned ID in the dictionary
        clients[socket] = client_id
    print(client_id)
    # Convert base64 data to PNG and save it to a folder
    filename = f"D:\pyCapture\image_{client_id}.png"
    image_data = base64.b64decode(base64_data)
    with open(filename, "wb") as image_file:
        image_file.write(image_data)

    print(f"Saved image to: {filename}")

    # Send reply back to client with its assigned ID
    socket.send(client_id.encode())
