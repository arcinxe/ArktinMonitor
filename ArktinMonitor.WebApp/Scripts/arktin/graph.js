var log = logs[0];
var graph = document.getElementById("graph");
var date = new Date(log.StartTime);
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
    //var header = document.getElementById("details-header");
    //header.innerHTML = Math.max(document.documentElement.clientWidth, window.innerWidth || 0);
    //ctx = canvas.getContext('2d');

    // resize the canvas to fill browser window dynamically
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
        ctx.beginPath();
        ctx.strokeStyle = "black";
        ctx.moveTo(0, 0);
        ctx.lineTo(canvas.width, canvas.height);
        ctx.moveTo(canvas.width, 0);
        ctx.lineTo(0, canvas.height);
        ctx.stroke();
        //ctx.font = "20px Roboto, sans-serif";
        //ctx.strokeText(date.toTimeString(), 20, 20);
        //ctx.strokeText(canvas.width + "x" + canvas.height, 20, 90);
        ctx.beginPath();
        ctx.setLineDash([5, 5]);
        ctx.strokeStyle = "rgba(255,255,255,0.2)";
        ctx.lineWidth = 0.5;
        ctx.textAlign = "center";
        ctx.textBaseline = "middle";
        ctx.fillStyle = "rgba(255,255,255,0.5)";
        ctx.font = "35px Roboto, sans-serif";
        ctx.fillText(canvas.width + "x" + canvas.height, width / 2, height / 2);
        ctx.font = "12px Roboto, sans-serif";
        ctx.fillText("window.innerWidth: " + window.innerWidth, width / 2, 10);
        var div2 = document.getElementById("div-graph");
        ctx.fillText("window.devicePixelRatio: " + window.devicePixelRatio, width / 2, 25);
        ctx.fillText("canvas.style.width: " + canvas.style.width, width / 2, 40);
        ctx.textBaseline = "bottom";
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
            ctx.fillText(hour, latitude, Math.floor(height * 0.98));
        }
        //debugger;
        logs.forEach(function (log) {
            console.log(log.User);
            ctx.fillStyle = "rgba(250,101,33,0.3)";
            var startTime = new Date(log.StartTime);
            console.log(startTime.getHours() + " : " + startTime.getMinutes());
            var startDrawingPosition = (((startTime.getHours() * 60 * 60)
                + startTime.getMinutes() * 60)
                + startTime.getSeconds()) / 86400 * canvas.width;
            var widthOfLog =
                (log.Duration / 86400) * canvas.width;
            ctx.fillRect(startDrawingPosition, 10, widthOfLog, 50);
        });
        ctx.stroke();
    }
};