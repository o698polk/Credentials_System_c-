// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function verificarClaves() {
    var correo = document.getElementById("correo").value;
    var clave1 = document.getElementById("clave1").value;
    var clave2 = document.getElementById("clave2").value;
    var miBoton = document.getElementById("btncreate");

    var miArray = correo.split("@");
   // && miArray[1] == "utelvt.edu.ec"
    if (clave1 === clave2 ) {
        // Mostrar el botón
        miBoton.style.display = "block";
       
    } else {
        //Ocultar 
        miBoton.style.display = "none";
    }
}
function verificarClaves1() {

    var clave1 = document.getElementById("clave3").value;
    var clave2 = document.getElementById("clave4").value;
    var miBoton = document.getElementById("btncreate1");

    if (clave1 === clave2) {
        // Mostrar el botón
        miBoton.style.display = "block";

    } else {
        //Ocultar 
        miBoton.style.display = "none";
    }
}

//-------------------------------------///
$(function () {
    var slides = $('.slides'),
        images = slides.find('img');

    images.each(function (i) {
        $(this).attr('data-id', i + 1);
    })

    var typed = new Typed('.typed-words', {
        strings: ["¿Qué esperas? ", " Actualízate", " Vamos","Úsalo ya"],
        typeSpeed: 80,
        backSpeed: 80,
        backDelay: 4000,
        startDelay: 1000,
        loop: true,
        showCursor: true,
        preStringTyped: (arrayPos, self) => {
            arrayPos++;
            console.log(arrayPos);
            $('.slides img').removeClass('active');
            $('.slides img[data-id="' + arrayPos + '"]').addClass('active');
        }

    });
})


//-------------------------------------------------//

const $boton = document.querySelector("#btnCapturar"),
    $objetivo = document.querySelector("#carnetid"),
    $contenedorCanvas = document.querySelector("#contenedorCanvas");


$boton.addEventListener("click", () => {
    html2canvas($objetivo)
        .then(canvas => {

            $contenedorCanvas.appendChild(canvas);
        });
    setTimeout(function () {
        $objetivo.style.display = "none";
        window.print();

   

    }, 2000);
    setTimeout(function () {

        $objetivo.style.display = "block";

    }, 10000);
});
window.addEventListener("afterprint", function () {
    // Realizar alguna acción después de que se complete la impresión
    window.location.reload();
});



//----------------------------------------------------//

// Obtener el div donde se mostrará la hora
var divHora = document.getElementById("div-hora");

// Función para actualizar la hora
function actualizarHora() {
    // Obtener la hora actual
    var fecha = new Date();
    var horas = fecha.getHours();
    var minutos = fecha.getMinutes();
    var segundos = fecha.getSeconds();
    var año = fecha.getFullYear();
    var mes = fecha.getMonth() + 1; // Se suma 1 porque los meses empiezan en 0
    var dia = fecha.getDate();
    // Formatear la hora en el formato deseado
    var horaFormateada = dia + "/" + mes + "/" + año +"/"+ horas + ":" + minutos + ":" + segundos;

    // Mostrar la hora en el div correspondiente
    divHora.innerHTML = horaFormateada;
}

// Actualizar la hora cada segundo
setInterval(actualizarHora, 1000);

/*-----------------------------------------------------------------*/

function ElegirFaultadColor() {
    let facultadSelect = $("#Facultades").attr("nfacul");
    let CARNET = document.getElementById('carnetfondo');
    let fondodatos = document.getElementById('fondodata');
    let nfacult = "";
    let ncolor = "";
    if (facultadSelect == "Facultad de Ingenierías") {
        nfacult = "FACI.jpeg";
        ncolor = "#9fc7c5";
    } else
        if (facultadSelect == "Facultad de la Pedagogía") {
            nfacult = "FACSOS.jpg";
            ncolor = "#ff986b";
        } else
            if (facultadSelect == "Facultad de Ciencias Administrativas y Económicas") {
                nfacult = "FACAE.jpeg";
                ncolor = "#469ef2";
            } else
                if (facultadSelect == "Facultad de Ciencias Sociales y de Servicios") {
                    nfacult = "FACPED.jpeg";
                    ncolor = "#dbc9a6";
                } else
                    if (facultadSelect == "Facultad de Ciencias Agropecuarias") {
                        nfacult = "FACAP.jpeg";
                        ncolor = "#9debab";
                    }
    console.log(facultadSelect);


    CARNET.style.backgroundImage = "url(/modelo/" + nfacult + ")";
    fondodatos.style.backgroundColor = ncolor;
}
ElegirFaultadColor();




//-------------------CODIGO DE LA PIZARRA------------------

var canvas = document.getElementById("canvas");
var ctx = canvas.getContext('2d');
var react = canvas.getBoundingClientRect();
var x = 0, y = 0, grosor = 1, color = "black", dibujando = false;

function defcolor(c) {
    color = c;
}

function defgrosor(g) {
    cgrosor = g;
}
canvas.addEventListener('mousedown', function (e) {
    x = e.clientX - react.left;
    y = e.clientY - react.top;
    dibujando = true;
});
canvas.addEventListener('mousemove', function (e) {
    if (dibujando === true) {
        dibujar(x, y, e.clientX - react.left, e.clientY - react.top);
        x = e.clientX - react.left;
        y = e.clientY - react.top;
    }
});
canvas.addEventListener('mouseup', function (e) {
    if (dibujando === true) {
        dibujar(x, y, e.clientX - react.left, e.clientY - react.top);
        x = 0;
        y = 0;
        dibujando = false;
    }
});

function dibujar(x1, y1, x2, y2) {
    ctx.beginPath();
    ctx.strokeStyle = color;
    ctx.lineWidth = grosor;
    ctx.moveTo(x1, y1);
    ctx.lineTo(x2, y2);
    ctx.stroke();
    ctx.closePath();

}
