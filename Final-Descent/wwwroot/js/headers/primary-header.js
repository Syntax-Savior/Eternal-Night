'use strict';

/**
 * Header Scroll Effect
 */
let lastScrollTop = 0;

const /** {NodeElement} */ $header = document.querySelector('header');
const /** {NodeElement} */ $scrollThreshold = 50;

window.addEventListener('scroll', function() {
  const /** {NodeElement} */ $scrollTop = window.pageYOffset || document.documentElement.$scrollTop;

  if ($scrollTop > lastScrollTop && $scrollTop > $scrollThreshold) {
    $header.classList.add('hidden');
  } else {
    $header.classList.remove('hidden');
  }

  if ($scrollTop > $scrollThreshold) {
    $header.classList.add('scrolled');
  } else {
    $header.classList.remove('scrolled');
  }

  lastScrollTop = $scrollTop;
});





/**
 * Header Profile Dropdown Effect
 */
let subMenu = document.getElementById("subMenu");

function toggleMenu(){
  subMenu.classList.toggle("drop-menu");
}





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
 * Header Delayed Appearance Effect
 */
gsap.from('.top-header', { opacity: 0, duration: 1, delay: 1, y: 10 })
gsap.from('.navigation', { opacity: 0, duration: 1, delay: 1.5, stagger: 0.2 })




