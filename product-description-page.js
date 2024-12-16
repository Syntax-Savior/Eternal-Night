'use strict';

/**
 * Sidebar Menu Open, Close, and Overlay Effect
 */
const /** {NodeElement} */ $menuIcon = document.getElementById('openMenu');
const /** {NodeElement} */ $closeIcon = document.getElementById('closeIcon');
const /** {NodeElement} */ $sidebarMenu = document.getElementById('sidebarMenu');
const /** {NodeElement} */ $overlay = document.getElementById('overlay');
const /** {NodeElement} */ $body = document.body;

$menuIcon.addEventListener('click', () => {
  $sidebarMenu.classList.add('active');
  $closeIcon.classList.add('visible');
  $overlay.classList.add('active');
  $body.classList.add('no-scroll');
});

$closeIcon.addEventListener('click', closeSidebar);
$overlay.addEventListener('click', closeSidebar);

function closeSidebar() {
  $sidebarMenu.classList.remove('active');
  $closeIcon.classList.remove('visible');
  $overlay.classList.remove('active');
  $body.classList.remove('no-scroll');
}





/**
 * Submenu Expanded List Effect
 */
document.addEventListener("DOMContentLoaded", () => {
  const /** {NodeElement} */ hmenuItems = document.querySelectorAll(".hmenu-items");
  const /** {NodeElement} */ backButtons = document.querySelectorAll(".submenu-back-button");
  const /** {NodeElement} */ menuItemsContainer = document.querySelector(".sidebar-menu-items");

  function openSubmenu(submenuId) {
    const /** {NodeElement} */ submenu = document.querySelector(`#${submenuId}`);
    if (submenu) {
      submenu.classList.add("active");
      menuItemsContainer.classList.add("move-left");
    }
  }

  function closeSubmenu(submenuId) {
    const /** {NodeElement} */ submenu = document.querySelector(`#${submenuId}`);
    if (submenu) {
      submenu.classList.remove("active");
      menuItemsContainer.classList.remove("move-left");
    }
  }

  hmenuItems.forEach(item => {
    item.addEventListener("click", e => {
      const /** {NodeElement} */ submenuId = item.dataset.trigger;
      openSubmenu(submenuId);
    });
  });

  backButtons.forEach(button => {
    button.addEventListener("click", e => {
      const /** {NodeElement} */ submenuId = button.dataset.back;
      closeSubmenu(submenuId);
    });
  });
});





/**
 * Submenu Dropdown List Effect
 */
const /** {NodeElement} */ $dropdownTrigger = document.getElementById("dropdownTrigger");
const /** {NodeElement} */ $dropdownMenu = document.getElementById("dropdownMenu");
const /** {NodeElement} */ $dropdownText = document.getElementById("dropdownText");
const /** {NodeElement} */ $dropdownIcon = document.getElementById("dropdownIcon");

$dropdownTrigger.addEventListener("click", () => {
  $dropdownMenu.classList.toggle("active");
  $dropdownTrigger.classList.toggle("active");

  if ($dropdownMenu.classList.contains("active")) {
    $dropdownText.textContent = "See less";
  } else {
    $dropdownText.textContent = "See all";
  }
});





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