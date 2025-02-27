# HEiKA-Game

## Introduction 

The integration of artificial intelligence (AI) into healthcare has opened new avenues for improving medical processes, from diagnosis to treatment. One area where AI can make an impact is in patient anamnesis—the process of collecting a patient’s medical history. Traditionally, this involves doctors talking to patients to gather important health information and details about their symptoms, which helps guide the next steps in their care. Modern computer systems, especially those powered by AI, can help with this process by conducting initial patient interviews digitally. This could reduce the routine workload for doctors and make clinical workflows more efficient. However, an important question remains: Does using such a system affect how patients respond, either positively or negatively?

This is the repository holding code for a game developed for a HEiKA project realizing the patient-doctor interaction via simulation of the medical history taking.  

![HEiKA Main Menu](/Imgs/heika_main_menu.png "HEiKA Main Menu")

## Run on Ubuntu

The game is already built for computers using Linux/Ubuntu and can be run by executing the build.x86_64 file. 

To play the game using two different computers do the following steps:

1. Open one instance of game 1 on one computer A  

2. Open another instance of game 2 on another computer B 

3. Find IP Address of Computer A (ifconfig) 

4. In Game 1, select a name (for example 'Doctor'), click on 'Join Lobby' and then click on 'Host Lobby'. Click on role 'Arzt' and on Button 'Ready'. 

5. In Game 2, select a name (for example 'Patient 1'), click on 'Join Lobby' and then click on 'Join Lobby'. Then, type in the IP-Address of Computer A. Click on join. Click on role 'Patient' and on Button 'Ready'. 

6. In any game instance, click on 'Start Game' to start the game.

## Run on Windows

We provide a build version of this project under the following link: https://heibox.uni-heidelberg.de/d/18affc00d7284f24882f/

Since the game is a multiplayer game, we will (at least) need two computers to play it properly. We will refer to the two computers played as computer A and computer B. Computer A and B will need to be on the same network (i.e., same WLAN or LAN network) for that to work. 

1. Computer A: Unzip/Extract the ServerV3.0.zip file. 

2. Computer A: Unzip/Extract the WindowsBuildV6.0.zip. 

3. Computer B: Unzip/Extract WindowsBuildV6.0.zip. 

4. Computer A: Go into the directory of the ServerV3.0 and start (double click) the application “Bachelorarbeit.exe”. 

5. Computer A: Open one instance of Game 1 by going into the WindowsBuildV6.0 directory and starting the application “Bachelorarbeit.exe”. 

6. Computer B: Open another instance of Game 2 by going into the WindowsBuildV6.0 directory and starting the application “Bachelorarbeit.exe”. 

7. Computer A: Find the IP Address of Computer A. They need to be in the same network (i.e., same LAN for example) for that to work. (https://support.microsoft.com/en-us/windows/find-your-ip-address-in-windows-f21a9bbc-c582-55cd-35e0-73431160a1b9) 

8. In Game 1, select a name (for example 'Doctor'), click on 'Join Lobby' and then click on “Join Lobby'. Click on “Join”. Click on role **'Arzt' **and on Button 'Ready'. 

9. In Game 2, select a name (for example 'Patient 1'), click on 'Join Lobby' and then click on 'Join Lobby'.  

    10. Then, type in the IP-Address of Computer A and the port number instead of the “localhost” text. This should be in the format of IP:Port number (for example: 192.168.1.231:7778 )  

    11. Click on join. Click on role 'Patient' and on Button 'Ready'. 

12. In Game 1, click on 'Start Game' to start the game. 

## Controls
The controls slightly vary based on the selection of the role (Doctor or Patient). 

## Patient
- Open the Chat by clicking on the monitor. 
- Click on the Text field in the - bottom left corner.  
- Send messages with "Enter".  
- You can close Chat via the X Button. 

## Doctor
- Open the Chat and get the mouse cursor by pressing the keyboard button "E". If “E” is pressed again, the chat closes. 
- Choose a message via the Dropdown Menu on the right or type in a message. Send a self-written message with "Enter". 
- If the auto chat feature should be used, press the "Auto Chat" button (more info on next section).    

## Autochat Feature
The Autochat Feature enables the doctor to ask questions in a section without the doctor typing in (or selecting) a question specifically. The doctor thus can physically leave the computer, and the patient can play the game on their own for one section. 

To use the Feature, the doctor clicks on the Autochat Button. Then immediately, the next question is asked. 

In patient view, a “Next Question” Button pops up. He can use that button to get the next question asked by the doctor. He can use that button until the corresponding section stops. 

## Others
The Transcript of the chat in the main level directory of the game in the file "ChatLog.txt".  