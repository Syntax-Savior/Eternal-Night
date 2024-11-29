'use strict';

document.addEventListener("DOMContentLoaded", () => {
  const previewButtons = document.querySelectorAll(".preview-btn");
  const modal = document.querySelector(".modal");
  const modalOverlay = document.getElementById("modalOverlay");
  const closeModalButton = document.getElementById("closePreview");
  const body = document.body;

  const openModal = () => {
    //modal.style.display = "flex";
    modal.classList.add("active");
    modalOverlay.classList.add("active");

    body.classList.add("no-scroll");
  };

  const closeModal = () => {
    //modal.style.display = "none";
    modal.classList.remove("active");
    modalOverlay.classList.remove("active");

    body.classList.remove("no-scroll");
  };

  previewButtons.forEach((button) => {
    button.addEventListener("click", (event) => {
      event.preventDefault();
      openModal();
    });
  });

  closeModalButton.addEventListener("click", closeModal);

  modalOverlay.addEventListener("click", closeModal);

  document.addEventListener("keydown", (event) => {
    if (event.key === "Escape" && modal.style.display === "flex") {
      closeModal();
    }
  });
});
