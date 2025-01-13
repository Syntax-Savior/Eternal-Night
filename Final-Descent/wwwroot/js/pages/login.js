// Header Scroll Direction Animation Start.
let lastScrollTop = 0;
  const header = document.querySelector('header');
  const scrollThreshold = 50; // Adjust this threshold as needed

  window.addEventListener('scroll', function() {
    const scrollTop = window.pageYOffset || document.documentElement.scrollTop;

    if (scrollTop > lastScrollTop && scrollTop > scrollThreshold) {
      // Scrolling down and past the threshold
      header.classList.add('hidden');
    } else {
      // Scrolling up or at the top
      header.classList.remove('hidden');
    }

    lastScrollTop = scrollTop;
  });
// Header Scroll Direction Animation End.