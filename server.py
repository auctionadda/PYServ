import requests

url = "https://pyserv.onrender.com"
base64_data = "your_base64_data_here"

response = requests.post(url, data=base64_data)

if response.status_code == 200:
    print("Received:", response.text)
else:
    print("Request failed with status code:", response.status_code)
