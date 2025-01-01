'use strict';

/**
 * Customer Review Left Column Dropdown Effect
 */
document.addEventListener("DOMContentLoaded", function () {
  const /** {NodeElement} */ $expanderHeader = document.getElementById("expanderHeader");
  const /** {NodeElement} */ $expandedContent = document.getElementById("expandedContent");

  $expanderHeader.addEventListener("click", () => {
    if ($expandedContent.style.display === "none" || !$expandedContent.style.display) {
      $expandedContent.style.display = "block";
    } else {
      $expandedContent.style.display = "none";
    }

    $expanderHeader.classList.toggle("active");
  });
});





/**
 * Customer Review Right Column Dropdown Effect
 */
document.addEventListener("DOMContentLoaded", function () {
  const /** {NodeElement} */ $expanderElement = document.getElementById("expanderElement");
  const /** {NodeElement} */ $expandedText = document.getElementById("expandableText");
  const /** {NodeElement} */ $expanderPrompt = document.getElementById("expanderPrompt");

  $expanderElement.addEventListener("click", () => {
    if ($expandedText.style.display === "none" || !$expandedText.style.display) {
      $expandedText.style.display = "block";
      $expanderPrompt.textContent = "Read Less";
    } else {
      $expandedText.style.display = "none";
      $expanderPrompt.textContent = "Read More";
    }

    $expanderElement.classList.toggle("active");
  });
});




