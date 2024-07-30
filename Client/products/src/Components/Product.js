import React, { useState, useEffect } from "react";
import "./Product.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faPlus,faTrash } from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";

function Product() {
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const navigate = useNavigate();

  useEffect(() => {
    fetch("https://localhost:5001/api/Product/products")
      .then((res) => res.json())
      .then((products) => setProducts(products));
  }, []);

  // Calculate the products to display on the current page
  const indexOfLastItem = currentPage * itemsPerPage;
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  const currentProducts = products.slice(indexOfFirstItem, indexOfLastItem);

  const totalPages = Math.ceil(products.length / itemsPerPage);

  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };
// navigating to the AddProduct form page
const handleAddProduct = () => {
  navigate("/add-product");
}
// delete functionality
const handleDelete = async (productId) => {
  try {
    await fetch(`https://localhost:5001/api/Product/products/${productId}`, {
      method: "DELETE",
    });
    setProducts(products.filter((product) => product.id !== productId));
  } catch (error) {
    console.error("Error deleting product:", error);
  }
};

  return (
    <div className="products-container">
      <h1>PRODUCT LIST</h1>
      <button onClick={handleAddProduct} className="add-product-button">
        <FontAwesomeIcon icon={faPlus} /> Add Product
      </button>
      <div className="product-table">
        <table>
          <thead>
            <tr>
              <th>Name</th>
              <th>Category</th>
              <th>Price</th>
              <th>Stock Quantity</th>
              <th>Supplier</th>
              <th>Description</th>
              <th>Actions</th>
            </tr>
          </thead>
          <tbody>
            {currentProducts.map((product) => (
              <tr key={product.id}>
                <td>{product.productName}</td>
                <td>{product.category}</td>
                <td>{product.price}</td>
                <td>{product.stockQuantity}</td>
                <td>{product.supplier}</td>
                <td>{product.description}</td>
                <td>
                  <button
                    onClick={() => handleDelete(product.id)}
                    className="delete-button"
                  >
                    <FontAwesomeIcon icon={faTrash} />
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
        <div className="pagination">
          <button onClick={handlePreviousPage} disabled={currentPage === 1}>
            Previous
          </button>
          <span>
            Page {currentPage} of {totalPages}
          </span>
          <button
            onClick={handleNextPage}
            disabled={currentPage === totalPages}
          >
            Next
          </button>
        </div>
      </div>
    </div>
  );
}

export default Product;
