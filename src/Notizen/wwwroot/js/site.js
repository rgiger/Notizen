Array.from(document.getElementsByClassName("relativedate")).forEach(function(element) {
  element.textContent = moment(element.textContent, "DD.MM.YY HH:mm").fromNow();
});