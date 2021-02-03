﻿
const connection = new signalR.HubConnectionBuilder().withUrl("/chatHub").build();

async function start() {
    try {
        await connection.start();
        console.log("SignalR Connected");
    } catch (err) {
        console.log(err);
        setTimeout(start, 5000);
    }
};

connection.onclose(start);

start();




document.getElementById("sendMessageBtn").addEventListener('click', async function () {
    const message = document.getElementById("message").value;
    const name = document.getElementById("name").value;
    try {
        await connection.invoke("SendMessage", name, message);
    } catch (err) {
        console.log(err);
    }
})

connection.on("ReceiveMessage", (name, message) => {
    const rec_msg = name + ": " + message;
    const li = document.createElement("li");
    li.textContent = rec_msg; 
    document.getElementById("messageList").appendChild(li);
})
