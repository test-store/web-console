
    onload = inicia; 
    
    var cantidadCopos = 30; 
    
function inicia() {
  
  for(c=0; c<cantidadCopos; c++) {
        copo = document.createElement("div");
    copo.setAttribute("class", "nieve");
    document.getElementById("capa").appendChild(copo);
    }
  
    
  for(n=0; n<cantidadCopos; n++) {
        document.querySelectorAll("#capa .nieve")[n].addEventListener("transitionend", function () { caida(this) }, false);
  
    
    }
  
    
  setTimeout(function(){
    for(c=0; c<cantidadCopos; c++) {
        elCopo = document.querySelectorAll("#capa .nieve")[c];
    
    elCopo.style.left = Math.floor(Math.random()*100)+ "vw";
    
    elCopo.style.transition = "top 15ms linear";
    
    elCopo.style.top = elCopo.style.top = "1vh";
    }
  }, 15);
}


/*FUNCIÓN QUE SE REPITE PARA CADA COPO*/
function caida(T) {
    
        T.style.transition = "";
    
    T.style.top = "0";
  
    
  setTimeout(function(){
    
        T.style.transitionProperty = "left, top";
    
    T.style.transitionDelay = Math.floor(Math.random()*3000)+ "ms";
    
    T.style.transitionDuration = (Math.floor(Math.random()*15000) + 5000)+ "ms";
    
    T.style.transitionTimingFunction = "ease-in, ease-out";
   
    
    T.style.left = Math.floor(Math.random()*100)+ "vw";
    
    T.style.top = "110vh";
    }, 15);
  }
  
