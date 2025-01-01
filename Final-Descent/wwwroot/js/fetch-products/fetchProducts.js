async function fetchProducts() {
  try {
      const response = await fetch('https://localhost:7287/api/Test/get-products');
      if (!response.ok) {
          throw new Error('Failed to fetch products');
      }
      const products = await response.json();
      renderProducts(products);
  } catch (error) {
      console.error('Error fetching products:', error);
  }
}

function renderProducts(products) {
  const productsList = document.getElementById('productsList');
  productsList.innerHTML = '';

  products.forEach(product => {
      const productCard = `
            <div class="card">

                <div class="card-banner">
                    <a class="product" href="#" target="_blank">
                        <figure class="img-holder" style="--width: 585; --height: 390;">
                            <img src="${product.ImageUrl}" alt="${product.Title}" width="585" height="390" class="img-cover">
                        </figure>
                    </a>
                    ${product.IsNew ? '<span class="badge label-medium">New</span>' : ''}
                    <button class="icon-btn fav-btn" aria-label="add to favorite" data-toggle-btn>
                        <span class="material-symbols-rounded" aria-hidden="true">favorite</span>
                    </button>
                    <button class="icon-btn cart-btn" aria-label="add to cart">
                        <span class="material-symbols-outlined" aria-hidden="true">add_shopping_cart</span>
                    </button>
                </div>

                <div class="card-content">
                    <h3 class="title-small card-title" title="${product.Title}">
                        <a href="#">${product.Title}</a>
                    </h3>
                    <div class="product-review">
                        <div class="rating-wrapper">
                            <span class="material-symbols-rounded" aria-hidden="true">star</span>
                            <span class="material-symbols-rounded" aria-hidden="true">star</span>
                            <span class="material-symbols-rounded" aria-hidden="true">star</span>
                            <span class="material-symbols-rounded" aria-hidden="true">star</span>
                            <span class="material-symbols-rounded" aria-hidden="true">star_half</span>
                        </div>
                        <span class="title-small rating-text">${product.SoldCount || 'N/A'} sold</span>
                        ${product.TopSelling ? `<img src="../Assets/Images/Icons/Top-Selling.png" alt="" class="top-selling">` : ''}
                    </div>
                    <div class="product-price">
                        <div class="discounted">
                            <span class="currency">${product.Currency}</span>
                            <span class="amount">${Math.floor(product.Price)}</span>
                            <span class="decimal-point">.</span>
                            <span class="decimals">${String(product.Price % 1).substring(2)}</span>
                        </div>
                        <div class="original">
                            <span class="striked-through">${product.Currency}${product.OriginalPrice}</span>
                        </div>
                    </div>
                    <div class="discount-details">
                        <img src="../Assets/Images/Icons/Black-Friday.png" alt="" class="black-friday">
                        <span class="divider"></span>
                        <img src="../Assets/Images/Icons/Discount-Arrow.png" alt="" class="discount-arrow">
                        <span class="save-up">${product.DiscountText}</span>
                    </div>
                    ${product.ChoiceText ? `
                        <div class="shipping-choice">
                            <img src="../Assets/Images/Icons/Choice.png" alt="" class="choice">
                            <span class="free-shipping">${product.ChoiceText}</span>
                        </div>` : ''}
                </div>

                <div class="card-meta-list">
                    <div class="preview">
                        <a href="#" class="btn btn-outline">
                            <span class="label-medium">See Preview</span>
                        </a>
                    </div>
                    <div class="similar-items">
                        <a href="#" class="btn btn-outline">
                            <span class="label-medium">Similar Items</span>
                        </a>
                    </div>
                </div>
            </div>
      `;
      productsList.innerHTML += productCard;
  });
}

document.addEventListener('DOMContentLoaded', fetchProducts);