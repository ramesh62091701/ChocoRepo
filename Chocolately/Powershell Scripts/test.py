def escape_string(input_string):
    escaped_string = input_string.replace("\\", "\\\\").replace("\"", "\\\"")
    return escaped_string

# Example usage
input_string = r'/SqlInstance:sonatashrdco'
escaped_string = escape_string(input_string)
print(escaped_string)