﻿// wwwroot/js/chatUtils.js
function scrollToBottom(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollTop = element.scrollHeight;
    }
}