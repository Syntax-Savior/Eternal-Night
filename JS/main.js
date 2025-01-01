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
 * Background Video Slider Effect
 */
const /** {NodeElement} */ $slides = document.querySelectorAll(".video-slide");
const /** {NodeElement} */ $contents = document.querySelectorAll(".content");
const /** {NodeElement} */ $buttons = document.querySelectorAll(".slider-controller");

let $currentSlide = 0;
let $slideInterval;

const /** {NodeElement} */ $sliderNav = function (manual) {
  clearInterval($slideInterval);
  $currentSlide = manual;
  $startAutoSlide();

  $slides.forEach((slide) => {
    slide.classList.remove("active");
  });

  $contents.forEach((content) => {
    content.classList.remove("active");
  });

  $buttons.forEach((button) => {
    button.classList.remove("active");
  });

  $slides[manual].classList.add("active");
  $contents[manual].classList.add("active");
  $buttons[manual].classList.add("active");
};

const /** {NodeElement} */ $nextSlide = function () {
  $currentSlide = ($currentSlide + 1) % $slides.length;
  $sliderNav($currentSlide);
};

const /** {NodeElement} */ $startAutoSlide = function () {
  $slideInterval = setInterval($nextSlide, 10000);
};

$startAutoSlide();

$buttons.forEach((button, i) => {
  button.addEventListener("click", () => {
    $sliderNav(i);
  });
});

let $inactiveTimeout;
document.addEventListener("click", function () {
  clearTimeout($inactiveTimeout);
  clearInterval($slideInterval);
  $inactiveTimeout = setTimeout(() => {
    $startAutoSlide();
  }, 5000);
});





/**
 * Product Image Cursor Movement Effect
 */
document.addEventListener('mousemove', movingImage);
function movingImage(e){
  this.querySelectorAll('.movingImage').forEach(layer => {
    const /** {NodeElement} */ $speed = layer.getAttribute('data-speed')

    const /** {NodeElement} */ $x = (window.innerWidth - e.pageX*$speed)/120
    const /** {NodeElement} */ $y = (window.innerWidth - e.pageY*$speed)/120

    layer.style.transform = `translateX(${$x}px) translateY(${$y}px)`
  });
};





/**
 * Header & Hero Delayed Appearance Effect
 */
gsap.from('.top-header', { opacity: 0, duration: 1, delay: 1, y: 10 })
gsap.from('.navigation', { opacity: 0, duration: 1, delay: 1.5, stagger: 0.2 })
gsap.from('.product-name', { opacity: 0, duration: 1, delay: 1.6, y: 30, stagger: 0.4 })
gsap.from('.product-description', { opacity: 0, duration: 1, delay: 1.8, y: 30, stagger: 0.4 })
gsap.from('.learn-more', { opacity: 0, duration: 1, delay: 2.1, y: 30, stagger: 0.4 })
gsap.from('.product-image', { opacity: 0, duration: 1, delay: 2.6, y: 30, stagger: 0.4 })
//gsap.from('.media-icons', { opacity: 0, duration: 1, delay: 2.8 })
gsap.from('.slider-navigation', { opacity: 0, duration: 1, delay: 3, y: 30, stagger: 0.5 })





/**
 * Main Section Categories Slider Effect
 */
const /** {NodeElement} */ $leftArrow = document.getElementById("leftArrow");
const /** {NodeElement} */ $rightArrow = document.getElementById("rightArrow");
const /** {NodeElement} */ $categoriesWrapper = document.getElementById("categoriesWrapper");
const /** {NodeElement} */ $fadeLeft = document.querySelector(".fade-overlay-left");
const /** {NodeElement} */ $fadeRight = document.querySelector(".fade-overlay-right");

const /** {NodeElement} */ $categoryColumns = document.querySelectorAll(".category-column");
const /** {Number} */ $columnWidth = $categoryColumns[0].offsetWidth;
//const /** {Number} */ $columnWidth = 175;
const /** {Number} */ $columnsPerScroll = 5;

function getMaxScrollLeft() {
  return $categoriesWrapper.scrollWidth - $categoriesWrapper.clientWidth;
}

function updateArrowsAndFades() {
  const /** {NodeElement} */ $maxScrollLeft = getMaxScrollLeft();
  
  if (Math.ceil($categoriesWrapper.scrollLeft) > 0) {
    $leftArrow.style.display = "block";
    $fadeLeft.style.display = "block";
  } else {
    $leftArrow.style.display = "none";
    $fadeLeft.style.display = "none";
  }

  if ($categoriesWrapper.scrollLeft >= $maxScrollLeft - 1) {
    $rightArrow.style.display = "none";
    $fadeRight.style.display = "none";
  } else {
    $rightArrow.style.display = "block";
    $fadeRight.style.display = "block";
  }
}

$rightArrow.addEventListener("click", () => {
  const /** {NodeElement} */ $maxScrollLeft = getMaxScrollLeft();
  const /** {NodeElement} */ $scrollAmount = Math.min($columnsPerScroll * $columnWidth, $maxScrollLeft - $categoriesWrapper.scrollLeft);

  $categoriesWrapper.scrollLeft += $scrollAmount;
  updateArrowsAndFades();
});

$leftArrow.addEventListener("click", () => {
  const /** {NodeElement} */ $scrollAmount = Math.min($columnsPerScroll * $columnWidth, $categoriesWrapper.scrollLeft);

  $categoriesWrapper.scrollLeft -= $scrollAmount;
  updateArrowsAndFades();
});

let isScrolling;
$categoriesWrapper.addEventListener("scroll", () => {
  window.clearTimeout(isScrolling);
  isScrolling = setTimeout(updateArrowsAndFades, 50);
});

updateArrowsAndFades();





/**
 * Products Card Button Effects
 */
const /** {NodeList} */ $toggleBtns = document.querySelectorAll("[data-toggle-btn]");

$toggleBtns.forEach($toggleBtn => {
  $toggleBtn.addEventListener("click", () => {
    $toggleBtn.classList.toggle("active");
  });
});





/**
 * Testimonials Hover/Slider Effect
 */
const /** {NodeElement} */ $testimonialsContainer = document.querySelector(".testimonials");
const /** {NodeElement} */ $sliderContents = document.querySelectorAll(".slider-content");
const /** {NodeElement} */ $slideBackward = document.getElementById("slideBackward");
const /** {NodeElement} */ $slideForward = document.getElementById("slideForward");

const /** {NodeElement} */ $firstSlide = $sliderContents[0].cloneNode(true);
const /** {NodeElement} */ $lastSlide = $sliderContents[$sliderContents.length - 1].cloneNode(true);
$testimonialsContainer.appendChild($firstSlide);
$testimonialsContainer.insertBefore($lastSlide, $testimonialsContainer.firstChild);

let currentSlide = 1;
const /** {Number} */ $totalSlides = $sliderContents.length + 2;

const /** {Number} */ $slideWidth = $sliderContents[0].offsetWidth;
$testimonialsContainer.style.transform = `translateX(${-currentSlide * $slideWidth}px)`;

function updateSlidePosition() {
  $testimonialsContainer.style.transform = `translateX(${-currentSlide * $slideWidth}px)`;
  $testimonialsContainer.style.transition = "transform 0.8s ease";
}

function handleLooping() {
  $testimonialsContainer.style.transition = "none";
  if (currentSlide === 0) {
    currentSlide = $sliderContents.length;
    $testimonialsContainer.style.transform = `translateX(${-currentSlide * $slideWidth}px)`;
  } else if (currentSlide === $totalSlides - 1) {
    currentSlide = 1;
    $testimonialsContainer.style.transform = `translateX(${-currentSlide * $slideWidth}px)`;
  }
}

$slideForward.addEventListener("click", () => {
  currentSlide++;
  updateSlidePosition();
  setTimeout(handleLooping, 800);
});

$slideBackward.addEventListener("click", () => {
  currentSlide--;
  updateSlidePosition();
  setTimeout(handleLooping, 800);
});

$slideBackward.addEventListener("mouseover", () => {
  $testimonialsContainer.style.transform = `translateX(${
    -(currentSlide * $slideWidth) + 70
  }px)`;
  $testimonialsContainer.style.transition = "transform 0.3s ease";
});
$slideBackward.addEventListener("mouseout", updateSlidePosition);

$slideForward.addEventListener("mouseover", () => {
  $testimonialsContainer.style.transform = `translateX(${
    -(currentSlide * $slideWidth) - 70
  }px)`;
  $testimonialsContainer.style.transition = "transform 0.3s ease";
});
$slideForward.addEventListener("mouseout", updateSlidePosition);

window.addEventListener("resize", () => {
  $testimonialsContainer.style.transition = "none";
  $testimonialsContainer.style.transform = `translateX(${-currentSlide * $sliderContents[0].offsetWidth}px)`;
});





/**
 * Widget Scroll Up Button Effect
 */
const /** {NodeElement} */ $scrollTopIcon = document.getElementById("scrollTopIcon");
const /** {NodeElement} */ $rightWidgetWrapper = document.querySelector(".right-widgets.wrapper");
const /** {NodeElement} */ $brLine = document.querySelector(".br-line");

window.addEventListener("scroll", () => {
  if (window.scrollY > 200) {
    $scrollTopIcon.classList.add("show");
    $brLine.style.margin = "8px 0";
  } else {
    $scrollTopIcon.classList.remove("show");
    $brLine.style.margin = "0";
  }
});

$scrollTopIcon.addEventListener("click", () => {
  window.scrollTo({ top: 0, behavior: "smooth" });
});




