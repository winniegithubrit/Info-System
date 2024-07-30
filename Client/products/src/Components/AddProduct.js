import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./AddProduct.css";

function AddProduct() {
  const [productName, setProductName] = useState("");
  const [category, setCategory] = useState("");
  const [price, setPrice] = useState("");
  const [stockQuantity, setStockQuantity] = useState("");
  const [supplier, setSupplier] = useState("");
  const [description, setDescription] = useState("");
  const [existingProducts, setExistingProducts] = useState([]);
  // state for error messages
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    // Fetch existing products to check for duplicates
    fetch("https://localhost:5001/api/Product/products")
      .then((res) => res.json())
      .then((products) => setExistingProducts(products))
      .catch((error) => console.error("Error fetching products:", error));
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Check for duplicated products with the same name and  category
    const duplicate = existingProducts.find(
      (product) =>
        product.productName === productName && product.category === category
    );

    if (duplicate) {
      setError("A product with the same name and category already exists.");
      // if there is duplication the error message is sent and the execution stopped
      return;
    }
// new product object
    const newProduct = {
      productName,
      category,
      price: parseFloat(price),
      stockQuantity: parseInt(stockQuantity),
      supplier,
      description,
    };
// post request to add a new product
    try {
      await fetch("https://localhost:5001/api/Product/products", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(newProduct),
      });
      setError(""); 
    
      // navigate("/");
    } catch (error) {
      console.error("Error adding product:", error);
    }
  };

  return (
    <div className="add-product-container">
      <h2>Add Product</h2>
      {error && <p className="error">{error}</p>}
      <form onSubmit={handleSubmit}>
        <div className="form-group">
          <label>Name</label>
          <input
            type="text"
            value={productName}
            onChange={(e) => setProductName(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Category</label>
          <input
            type="text"
            value={category}
            onChange={(e) => setCategory(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Price</label>
          <input
            type="number"
            step="10"
            value={price}
            onChange={(e) => setPrice(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Stock Quantity</label>
          <input
            type="number"
            value={stockQuantity}
            onChange={(e) => setStockQuantity(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Supplier</label>
          <input
            type="text"
            value={supplier}
            onChange={(e) => setSupplier(e.target.value)}
            required
          />
        </div>
        <div className="form-group">
          <label>Description</label>
          <textarea
            value={description}
            onChange={(e) => setDescription(e.target.value)}
            required
          />
        </div>
        <button type="submit">Add Product</button>
        <button
          type="button"
          className="back-button"
          onClick={() => navigate("/")}
        >
          Back
        </button>
      </form>
    </div>
  );
}

export default AddProduct;
