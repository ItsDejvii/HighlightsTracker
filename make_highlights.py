import csv
import os
import sys

zacni_prej = 25
duration = 35

if len(sys.argv) < 4:
    print("Usage: python script.py <input_video> <timestamps> <video_num>")
    sys.exit(1)

input_video = sys.argv[1]
timestamps_file = sys.argv[2]
video_num = sys.argv[3]
output_folder = "highlights"
os.makedirs(output_folder, exist_ok=True)

ffprobe_cmd = f"""
ffprobe -v error -select_streams v:0 -show_entries stream=codec_name -of csv=p=0 {input_video}
"""
print(os.system(ffprobe_cmd))

with open(timestamps_file, newline="") as csvfile:
    reader = csv.reader(csvfile)
    # next(reader)  # Skip header

    for index, row in enumerate(reader, start=1):
        timestamp = row[0].replace(":", "_")  # Format HH_MM_SS
        highlight_type = row[1].strip().replace(" ", "_")  # Remove spaces
        highlight_name = row[2].strip().replace(" ", "_")  # Remove spaces

        h, m, s = map(int, row[0].split(":"))
        start_time = max(0, h * 3600 + m * 60 + s - zacni_prej)
        
        output_file = os.path.join(output_folder, f"{video_num}_highlight{index}_{timestamp}_{highlight_type}_{highlight_name}.mp4")
        print("Highlight", start_time, duration, output_file)

        # FFmpeg command: re-encode while keeping same codecs
        ffmpeg_cmd = f"""
        ffmpeg -i "{input_video}" -ss {start_time} -t {duration} -c copy "{output_file}"
        """
        
        print(f"Running: {ffmpeg_cmd}")
        os.system(ffmpeg_cmd)
