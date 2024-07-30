import React, { useState, useEffect } from "react";
import { useParams, useNavigate } from "react-router-dom";
import "./UpdateProduct.css";

function UpdateProductPage() {
  const { id } = useParams();
  const [product, setProduct] = useState(null);
  const [productName, setProductName] = useState("");
  const [category, setCategory] = useState("");
  const [price, setPrice] = useState("");
  const [stockQuantity, setStockQuantity] = useState("");
  const [supplier, setSupplier] = useState("");
  const [description, setDescription] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const fetchProduct = async () => {
      try {
        const response = await fetch(
          `https://localhost:5001/api/Product/products/${id}`
        );
        const data = await response.json();
        setProduct(data);
        setProductName(data.productName);
        setCategory(data.category);
        setPrice(data.price);
        setStockQuantity(data.stockQuantity);
        setSupplier(data.supplier);
        setDescription(data.description);
      } catch (error) {
        console.error("Error fetching product:", error);
      }
    };

    fetchProduct();
  }, [id]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const updatedProduct = {
      ...product,
      productName,
      category,
      price: parseFloat(price),
      stockQuantity: parseInt(stockQuantity),
      supplier,
      description,
    };

    try {
      await fetch(`https://localhost:5001/api/Product/products/${id}`, {
        method: "PUT",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify(updatedProduct),
      });
      //navigate("/");
    } catch (error) {
      console.error("Error updating product:", error);
    }
  };

  const handleBack = () => {
    navigate("/");
  };

  return (
    <div className="update-product">
      <h2>Update Product</h2>
      {product && (
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
              step="0.01"
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
          <button type="submit">Update Product</button>
          <button type="button" onClick={handleBack}>
            Back
          </button>
        </form>
      )}
    </div>
  );
}

export default UpdateProductPage;
