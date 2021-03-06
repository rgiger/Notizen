Array.from(document.getElementsByClassName("relativedate")).forEach(function (element) {
  element.textContent = moment(element.textContent, "DD.MM.YYYY HH:mm").fromNow();
});

$("#datetimepicker").datetimepicker();

var forms = Array.from(document.getElementsByTagName('form'));
if (forms.length) {
  var warneVorDatenverlust = function (event) {
    if (Array.from(document.querySelectorAll('form input[type=text], form textarea')).filter(function (element) {
      return typeof element.value != "undefined" && element.value != element.defaultValue;
    }).length) {
      event.preventDefault();
    }
  }
  window.addEventListener("beforeunload", warneVorDatenverlust);
  forms.forEach(function (element) {
    element.addEventListener("submit", function () {
      if ($(element).valid()) {
        window.removeEventListener("beforeunload", warneVorDatenverlust);
      }
    });
  });
}
