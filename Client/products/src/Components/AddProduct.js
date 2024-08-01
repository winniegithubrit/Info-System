import React, { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import "./AddProduct.css";

function AddProduct() {
  const [formData, setFormData] = useState({
    productName: "",
    category: "",
    price: "",
    stockQuantity: "",
    supplier: "",
    description: "",
  });
  const [existingProducts, setExistingProducts] = useState([]);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    fetch("https://localhost:5001/api/Product/products")
      .then((res) => res.json())
      .then((products) => setExistingProducts(products))
      .catch((error) => console.error("Error fetching products:", error));
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const duplicate = existingProducts.find(
      (product) =>
        product.productName === formData.productName &&
        product.category === formData.category
    );

    if (duplicate) {
      setError("A product with the same name and category already exists.");
      return;
    }

    try {
      await fetch("https://localhost:5001/api/Product/products", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(formData),
      });
      setError("");
      navigate("/");
      setFormData({
        productName: "",
        category: "",
        price: "",
        stockQuantity: "",
        supplier: "",
        description: "",
      });
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
          <label>Product Name:</label>
          <input
            type="text"
            value={formData.productName}
            onChange={(e) =>
              setFormData({ ...formData, productName: e.target.value })
            }
            required
          />
        </div>
        <div className="form-group">
          <label>Category:</label>
          <input
            type="text"
            value={formData.category}
            onChange={(e) =>
              setFormData({ ...formData, category: e.target.value })
            }
            required
          />
        </div>
        <div className="form-group">
          <label>Price:</label>
          <input
            type="number"
            step="10"
            value={formData.price}
            onChange={(e) =>
              setFormData({ ...formData, price: e.target.value })
            }
            required
          />
        </div>
        <div className="form-group">
          <label>Stock Quantity:</label>
          <input
            type="number"
            value={formData.stockQuantity}
            onChange={(e) =>
              setFormData({ ...formData, stockQuantity: e.target.value })
            }
            required
          />
        </div>
        <div className="form-group">
          <label>Supplier:</label>
          <input
            type="text"
            value={formData.supplier}
            onChange={(e) =>
              setFormData({ ...formData, supplier: e.target.value })
            }
            required
          />
        </div>
        <div className="form-group">
          <label>Description:</label>
          <textarea
            value={formData.description}
            onChange={(e) =>
              setFormData({ ...formData, description: e.target.value })
            }
            required
          ></textarea>
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
