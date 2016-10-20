Array.from(document.getElementsByClassName("relativedate")).forEach(function (element) {
  element.textContent = moment(element.textContent, "DD.MM.YYYY HH:mm").fromNow();
});

var forms = Array.from(document.getElementsByTagName('form'));
if (forms.length) {
  var warneVorDatenverlust = function (event) {
    if (Array.from(document.getElementsByTagName('input')).filter(function (element) {
      return element.type == "text" && typeof element.value != "undefined" && element.value != element.defaultValue;
    }).length) {
      event.preventDefault();
    }
  }
  window.addEventListener("beforeunload", warneVorDatenverlust);
  forms.forEach(function (element) {
    element.addEventListener("submit", function () {
      window.removeEventListener("beforeunload", warneVorDatenverlust);
    });
  });
}