def escape_string(input_string):
    escaped_string = input_string.replace("\\", "\\\\").replace("\"", "\\\"")
    return escaped_string

# Example usage
input_string = r'/Hide /MaxThreads=8 /AuthType=0 /Site="sonatashrdsite" /verbose /LogPath="C:\Program Files (x86)\Common Files\US Group\Install Logs\ServerUpdater" /WorkPath="c:\installs\suwork\ServerUpdater'
escaped_string = escape_string(input_string)
print(escaped_string)