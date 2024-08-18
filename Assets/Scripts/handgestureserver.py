import cv2
import math
import mediapipe as mp
import socket

# Initialize Mediapipe
mp_drawing = mp.solutions.drawing_utils
mp_hands = mp.solutions.hands

# Initialize video capture
cap = cv2.VideoCapture(0)
# Check if camera opened successfully
if not cap.isOpened():
    print("Unable to open camera")
    exit()

# create sockets server

s = socket.socket()

PORT = 1234

s.connect(('127.0.0.1', PORT))

print("CONNECTED TO SERVER")

# Initialize Mediapipe Hands object
with mp_hands.Hands(static_image_mode=False, max_num_hands=2, min_detection_confidence=0.5) as hands:
    while True:
        # Read the frame from the video capture
        ret, frame = cap.read()
        if not ret:
            print("Error reading frame from camera")
            break
        # Convert the frame to RGB for Mediapipe
        frame_rgb = cv2.cvtColor(frame, cv2.COLOR_BGR2RGB)
        # Process the frame with Mediapipe
        results = hands.process(frame_rgb)
        # Draw hand landmarks on the frame
        if results.multi_hand_landmarks:
            for hand_landmarks in results.multi_hand_landmarks:
                mp_drawing.draw_landmarks(frame, hand_landmarks, mp_hands.HAND_CONNECTIONS,
                                          mp_drawing.DrawingSpec(color=(0, 255, 0), thickness=2, circle_radius=4),
                                          mp_drawing.DrawingSpec(color=(255, 0, 0), thickness=2, circle_radius=2))
                
            wrist_landmark = hand_landmarks.landmark[0]

            # Convert normalized coordinates to pixel values
            height, width, _ = frame.shape
            midpoint_landmarks = ((
                int(hand_landmarks.landmark[0].x * width),
                int(hand_landmarks.landmark[0].y * height)
            ),
            (
                int(hand_landmarks.landmark[5].x * width),
                int(hand_landmarks.landmark[5].y * height)
            ),
            (
                int(hand_landmarks.landmark[17].x * width),
                int(hand_landmarks.landmark[17].y * height)
            )
            )

            detection_finger_position = (
                int(hand_landmarks.landmark[16].x * width),
                int(hand_landmarks.landmark[16].y * height)
            ),

            midpoint_pixels = tuple(map(lambda y: int(sum(y) / float(len(y))), zip(*midpoint_landmarks)))
            midpoint_rel = (midpoint_pixels[0]/width, midpoint_pixels[1]/height)

            distance_detection_to_midpoint = math.sqrt(math.pow(detection_finger_position[0][0]-midpoint_pixels[0], 2) + math.pow(detection_finger_position[0][1]-midpoint_pixels[1], 2))

            cv2.circle(frame, midpoint_pixels, 5, (255, 0, 0)) # draw the midpoint

            if(distance_detection_to_midpoint < 20):
                s.send(f"1 {midpoint_rel[0]} {midpoint_rel[1]}".encode())
            else:
                s.send(f"0 {midpoint_rel[0]} {midpoint_rel[1]}".encode())
            
            #print(hand_position)
        # Display the frame
        #cv2.imshow("Hand Detection", frame)
        # Check for the 'q' key to exit
        if cv2.waitKey(1) == ord("q"):
            break
# Release the video capture and close all windows
cap.release()
cv2.destroyAllWindows()