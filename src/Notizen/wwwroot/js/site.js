Array.from(document.getElementsByClassName("relativedate")).forEach(function(element) {
  element.textContent = moment(element.textContent, "DD.MM.YYYY HH:mm").fromNow();
});