# [ArktinMonitor](https://arktin.azurewebsites.net)
Complex solution for monitoring, remote & parental control of a Windows PC through a web application. Some features include displaying user activity and computer parameters, sending both text and text-to-speech messages to the PC, killing processes, locking access to websites and apps.

[arktin.azurewebsites.net](https://arktin.azurewebsites.net)

#### Used technologies
* .NET
* Windows service
* WPF
* ASP[]().NET Web API
* ASP[]().NET MVC
* Entity Framework
* Azure SQL Server.
* SignalR
* Bootstrap

#### Overview of features
<details>
<summary>Logging into the apps</summary> 

**Desktop app**

After creating an account through the web app, the user can use those credential to log into the desktop app. As shown below, the desktop app also lets selecting an app to block for a selected user. Another functionality is choosing which users should be affected by the system. The credentials are stored for later use in Windows Credential Manager. 

![screen recording of logging into desktop app](https://i.imgur.com/16dnMlm.gif)

**Web app**

When the user has logged in via the desktop app, it becomes possible to access this computer through the web app. 

![screen recording of logging into the web app](https://imgur.com/w7aZdaE.gif)

</details>

<details>
<summary>Checking computer status</summary>

Web app displays basic computer details such as name, CPU, GPU, disk partitions. There is also a log that shows in real-time what service installed on target PC is currently doing.

</details>

<details>
<summary>Checking time spent by each user</summary>

A time of being logged in and being active is measured for each user of the computer. The web app allows checking both of that time values for all users of the PC. The graph on the bottom of the selected computer's page shows the distribution of computer usage over time.

![screen recording of checking time spent by each user](https://imgur.com/yg2ubN0.gif)

</details>

<details>
<summary>Setting up time limits, blocked apps and websites per user</summary> 

**Time limits**

Setting a time limit causes automatic logoff of the selected user after due time. There are text-to-speech reminders which fire up 20, 10, 5, 2 and 1 minutes before logoff will occur.

**Blocked websites**

Setting a website as blocked adds it to the hosts file and sets it to the loop-back address making it mostly inaccessible.

**Blocked apps**

Blocking an app beside using the desktop app can also be done through the web app. Doing it that way requires the user to input a direct path to the application. Blocking an app is implemented by periodically checking the processes list and killing it when it's running.

![screen recording of setting up time limits, blocked apps and websites per user](https://i.imgur.com/hljdlbb.gif)

</details>

<details>
<summary>Sending messages to the computer</summary> 

**Text messages**

The user can send text messages that will be displayed using a message box on the screen of the selected computer.

**Text-to-speech messages**

Another form of communication is sending a text message that will be then read out loud by the speech synthesizer. The application tries to select the first English voice it finds, otherwise will use any other voice.

![screen recording of sending text messages to the computer](https://i.imgur.com/aHeOLl0.gif)

</details>

<details>
<summary>Managing processes</summary> 

User can browse a list of all the processes running currently on the computer. System processes are marked with reddish names. Selecting the process opens a dialog that shows details such as path, copyright info, PID, the session in which it is running and also lets user kill the selected process. The refresh option refreshes the list of processes.

![screen recording of killing apps](https://i.imgur.com/uL8gXM8.gif)

</details>

<details>
<summary>Managing power state and user session</summary> 

The user can remotely simply lock their computer, or do some other available action. 

![screen recording of remotely locking the computer](https://i.imgur.com/1btKdtb.gif)

</details>
