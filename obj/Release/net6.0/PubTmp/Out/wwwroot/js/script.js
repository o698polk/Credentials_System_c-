const msgerForm = get(".msger-inputarea");
const msgerInput = get(".msger-input");
const msgerChat = get(".msger-chat");
let imgperfil = $("#idperfilimguser").attr("imgus");

const BOT_MSGS = [
    "Hola, cómo estás?",
    "Ohh... No puedo entender lo que intentas decir. Lo siento!",
    "Me gusta jugar juegos... pero no sé cómo jugar!",
    "Lo siento si mis respuestas no son relevantes.. :))",
    "Tengo sueño! :("];

let _respuesta = "";

const BOT_IMG = "https://pbs.twimg.com/profile_images/557902562959978496/YEBIqgVK_400x400.png";
//const PERSON_IMG = "https://cdn-icons-png.flaticon.com/512/2436/2436636.png";
const PERSON_IMG = imgperfil;
const BOT_NAME = "CHATBOT";
const PERSON_NAME = "Estudiante";

msgerForm.addEventListener("submit", event => {
    event.preventDefault();

    const msgText = msgerInput.value;
    if (!msgText) return;

    appendMessage(PERSON_NAME, PERSON_IMG, "right", msgText);


    $.ajax({
        url: "/chatbot/cahtutlvt",
        type: "POST",
        contentType: "application/json",
        data: JSON.stringify(msgText),
        success: function (respuesta) {
            console.log("La respuesta de la API es: ", respuesta);
            _respuesta = respuesta;
            botResponse();
        },
        error: function (error) {
            console.error("La solicitud POST falló: ", error);
        }
    });

    msgerInput.value = "";


});

function appendMessage(name, img, side, text) {
    //   Simple solution for small apps
    const msgHTML = `
    <div class="msg ${side}-msg">
      <div class="msg-img" style="background-image: url(${img})" loading="lazy"></div>

      <div class="msg-bubble">
        <div class="msg-info">
          <div class="msg-info-name">${name}</div>
          <div class="msg-info-time">${formatDate(new Date())}</div>
        </div>

        <div class="msg-text">${text}</div>
      </div>
    </div>
  `;

    msgerChat.insertAdjacentHTML("beforeend", msgHTML);
    msgerChat.scrollTop += 500;
}

function botResponse() {
    //const r = random(0, BOT_MSGS.length - 1);
    const msgText = _respuesta;//BOT_MSGS[r];
    const delay = msgText.split(" ").length * 100;

    setTimeout(() => {
        appendMessage(BOT_NAME, BOT_IMG, "left", msgText);
    }, delay);
}

// Utils
function get(selector, root = document) {
    return root.querySelector(selector);
}

function formatDate(date) {
    const h = "0" + date.getHours();
    const m = "0" + date.getMinutes();

    return `${h.slice(-2)}:${m.slice(-2)}`;
}

function random(min, max) {
    return Math.floor(Math.random() * (max - min) + min);
}


// Funcion para cambiar fondo segun la facultad

