var log = logs[0];
var graph = document.getElementById("graph");
//var date = new Date(log.StartTime);
window.onload = drawGraph;
//canvasContext = graph.getContext("2d");
//canvasContext.moveTo(0, 0);
//canvasContext.lineTo(100, 100);
//canvasContext.stroke();
//canvasContext.font = "30px Arial";
//canvasContext.strokeText(date.toTimeString(),10,50);
function drawGraph() {
    var canvas = document.getElementById("graph");
    var div = document.getElementById("div-graph");
    //var colors = ["rgba(255,23,68,", "rgba(255,145,0,", "rgba(255,234,0,", "rgba(118,255,3,", "rgba(0,229,255,", "rgba(41,121,255,", "rgba(117,117,117,"];


    // Filling users table.
    var users = [{ name: "", activeTime: 0, idleTime: 0, totalTime: 0 }];
    logs.forEach(function (log) {
        if (users.filter(u => u.name === log.User).length > 0) {
            var user = users.find(function (el) {
                return el.name === log.User;
            });
            user.totalTime += log.Duration;
            if (log.State === "Active") {
                user.activeTime += log.Duration;
            } else {
                user.idleTime += log.Duration;
            }
        } else {
            var activeTime = log.State === "Active" ? log.Duration : 0;
            var idleTime = log.State === "Idle" ? log.Duration : 0;
            users.push({ name: log.User, activeTime: activeTime, idleTime: idleTime, totalTime: log.Duration });
        }

    });
    // Sorting users array.
    users = users.filter(u => u.name !== "");
    users.sort(function (a, b) {
        return b.totalTime - a.totalTime;
    });

    // Generating array of colors.
    var amoutOfColors = users.length;
    var hslColors = [];
    for (var i = 0; i < amoutOfColors; i++) {
        var hueDifference = Math.floor(360 / amoutOfColors);
        hslColors.push(i * hueDifference + ",70%,60%");
    }

    console.table(hslColors);

    // Sorting users in on page and setting color bars.
    var count = 0;
    users.forEach(function (user) {

        var userId = "#user-" + user.name.toLowerCase();
        var temp = $(".user:eq(" + count + ") p").text();
        var currentUserDiv = $(userId);
        if ($(".user:eq(" + count + ") p").text() !== user.name) {
            currentUserDiv.insertBefore(".user:eq(" + count + ")");
        }


        //var activeTimeSpan = $("<span class='span-active-time'> Active time: " + user.activeTime + "</span>");
        //var idleTimeSpan = $("<span class='span-idle-time'> Idle time: " + user.idleTime + "</span>");
        //$(userId + " p").append(activeTimeSpan);
        //$(userId + " p").append(idleTimeSpan);
        //var color = colors[count]
        //var opacity =
        $(userId + " p").attr("title", timeFormat("Total time: ", user.totalTime));
        var highestTime = users[0].totalTime;

        var activeWidth = (user.activeTime / highestTime * 100);
        var activeProgress = $(".user:eq(" + count + ") .my-progress .progress-active");
        activeProgress.css("background", "hsla(" + hslColors[count] + ",0.6)");
        activeProgress.width(activeWidth + "%");
        activeProgress.attr("title", timeFormat("Active time: ", user.activeTime));

        var idleWidth = (user.idleTime / highestTime * 100);
        var idleProgress = $(".user:eq(" + count + ") .my-progress .progress-idle");
        idleProgress.css("background", "hsla(" + hslColors[count] + ",0.2)");
        idleProgress.width(idleWidth + "%");
        idleProgress.attr("title", timeFormat("Idle time: ", user.idleTime));

        var backgroundProgress = $(".user:eq(" + count + ") .my-progress .progress-background");
        backgroundProgress.width((100 - activeWidth - idleWidth) + "%");

        count++;
    });

    $('[data-toggle="tooltip"]').tooltip();


    //var colors = ["rgba(255,87,34,", "rgba(255,111,0,", "rgba(255,160,0,", "rgba(255,193,7,", "rgba(255,224,130,", "rgba(255,248,225,", "rgba(117,117,117,"];
    //var header = document.getElementById("details-header");
    //header.innerHTML = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
    //ctx = canvas.getContext('2d');

    // Resize the canvas to fill browser window dynamically
    window.addEventListener("resize", resizeCanvas, false);

    function resizeCanvas() {

        //canvas.width = div.clientWidth - 20;// Minus padding
        canvas.width = (div.clientWidth * window.devicePixelRatio) - 20;
        canvas.height = (150 * window.devicePixelRatio);
        canvas.style.width = div.clientWidth - 20 + "px";
        //canvas.style.height = 150+"px";
        // canvas.height = div.clientHeight;
        /**
         * Your drawings need to be inside this function otherwise they will be reset when
         * you resize the browser window and the canvas goes will be cleared.
         */
        drawStuff();
    }
    resizeCanvas();

    function drawStuff() {
        // do your drawing stuff here
        var ctx = canvas.getContext("2d");

        var width = canvas.width;
        var height = canvas.height;
        // Debugging drawings
        //ctx.beginPath();
        //ctx.strokeStyle = "black";
        //ctx.moveTo(0, 0);
        //ctx.lineTo(canvas.width, canvas.height);
        //ctx.moveTo(canvas.width, 0);
        //ctx.lineTo(0, canvas.height);
        //ctx.stroke();
        //ctx.font = "35px Roboto, sans-serif";
        //ctx.fillText(canvas.width + "x" + canvas.height, width / 2, height / 2);
        //ctx.font = "12px Roboto, sans-serif";
        //ctx.fillText("window.innerWidth: " + window.innerWidth, width / 2, 10);
        //var div2 = document.getElementById("div-graph");
        //ctx.fillText("window.devicePixelRatio: " + window.devicePixelRatio, width / 2, 25);
        //ctx.fillText("canvas.style.width: " + canvas.style.width, width / 2, 40);
        //ctx.textBaseline = "bottom";
        ctx.beginPath();
        ctx.setLineDash([5, 5]);
        ctx.strokeStyle = "rgba(255,255,255,0.2)";
        ctx.lineWidth = 0.5;
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillStyle = "rgba(255,255,255,0.5)";
        // Draws grid with timestamps.
        for (var i = 0; i < 24; i++) {
            var latitude = Math.floor(width / 24 * i);
            var hour = i < 10 ? "0" + i : i;
            ctx.moveTo(latitude, 0);
            ctx.lineTo(latitude, height - 1);
            if (i === 0) continue;
            if (width > 900) hour += ":00";
            if (width < 400) ctx.font = "10px Roboto, sans-serif";
            if (width < 300) ctx.font = "8px Roboto, sans-serif";
            ctx.fillText(hour, latitude, Math.floor(height * 0.97));
        }

        // Drawing logs.
        logs.forEach(function (log) {
            var user = users.find(function (el) {
                return el.name === log.User;
            });
            var index = users.indexOf(user);
            if (user == undefined) {
                ctx.fillStyle = "hsla(0,0%,80%,0.5)";
            } else {
                var opacity = log.State === "Active" ? 0.6 : 0.2;
                ctx.fillStyle = "hsla(" + hslColors[index] + "," + opacity + ")";
            }

            var startTime = new Date(log.StartTime);
            var startDrawingPosition = (((startTime.getHours() * 60 * 60)
                + startTime.getMinutes() * 60)
                + startTime.getSeconds()) / 86400 * canvas.width;
            var widthOfLog =
                (log.Duration / 86400) * canvas.width;
            ctx.fillRect(startDrawingPosition, 10, widthOfLog, height * 0.8);
        });
        console.table(logs);
        console.table(users);
        ctx.stroke();
    }
};

function timeFormat(text, time) {
    var seconds = time % 60;
    var minutes = Math.floor(time % 3600 / 60);
    var hours = Math.floor(time / 3600);
    var hoursText = hours === 0 ? "" : hours + "h ";
    return text + " " + hoursText + minutes + "m " + seconds + "s";
}