from flask import Flask, jsonify, request
import json
import os

app = Flask(__name__)

class UniqueStringGenerator:
    def __init__(self, state_file='prefix_state.json'):
        # Define the prefixes and initialize all counts to 0
        self.prefixes = {
            'TWL': 0,
            'CLA': 0,
            'CHR': 0,
            'CUA': 0,
            'GBL': 0,
            'RXC': 0
        }
        self.state_file = state_file
        self.load_state()

    def load_state(self):
        if os.path.exists(self.state_file):
            with open(self.state_file, 'r') as file:
                self.prefixes = json.load(file)

    def save_state(self):
        with open(self.state_file, 'w') as file:
            json.dump(self.prefixes, file)

    def get_next(self, prefix):
        if prefix not in self.prefixes:
            raise ValueError(f"Invalid prefix: {prefix}")
        current_number = self.prefixes[prefix]
        number_str = f'{current_number:05d}'
        self.prefixes[prefix] += 1
        self.save_state()
        return f'{prefix}{number_str}'

generator = UniqueStringGenerator()

@app.route('/get_number', methods=['GET'])
def get_number():
    prefix = request.args.get('prefix')
    if not prefix:
        return jsonify({"error": "Prefix is required"}), 400

    try:
        next_string = generator.get_next(prefix.upper())
        return jsonify({"number": next_string})
    except ValueError as e:
        return jsonify({"error": str(e)}), 400

if __name__ == "__main__":
    app.run(host='0.0.0.0', port=5000)
