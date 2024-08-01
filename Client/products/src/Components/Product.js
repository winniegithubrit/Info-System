import React, { useState, useEffect } from "react";
import "./Product.css";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
  faPlus,
  faTrash,
  faPenToSquare,
} from "@fortawesome/free-solid-svg-icons";
import { useNavigate } from "react-router-dom";

function Product() {
  const [products, setProducts] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const itemsPerPage = 5;
  const navigate = useNavigate();

  // Fetch products function
  const fetchProducts = async () => {
    try {
      const response = await fetch(
        "https://localhost:5001/api/Product/products"
      );
      const data = await response.json();
      setProducts(data);
      console.log("refreshing data:", new Date());
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };

  // calling the fetch method
  useEffect(() => {
    fetchProducts();
    // Fetch products every 5 seconds
    const interval = setInterval(() => {
      fetchProducts();
      
    }, 5000); 

    
    return () => clearInterval(interval);
  }, []);

  // Calculate the index of the last item on the current page
  const indexOfLastItem = currentPage * itemsPerPage;
  // Calculate the index of the first item on the current page
  const indexOfFirstItem = indexOfLastItem - itemsPerPage;
  // Get a portion of the products array to be displayed on the current page
  const currentProducts = products.slice(indexOfFirstItem, indexOfLastItem);
  // Calculate the total number of pages
  const totalPages = Math.ceil(products.length / itemsPerPage);

  // Handle the previous page
  const handlePreviousPage = () => {
    if (currentPage > 1) {
      setCurrentPage(currentPage - 1);
    }
  };

  // Handle the next page
  const handleNextPage = () => {
    if (currentPage < totalPages) {
      setCurrentPage(currentPage + 1);
    }
  };

  const handleAddProduct = () => {
    navigate("/add-product");
  };

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

  const handleEdit = (productId) => {
    navigate(`/update-product/${productId}`);
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
                  <div className="actions">
                    <button
                      onClick={() => handleDelete(product.id)}
                      className="delete-button"
                    >
                      <FontAwesomeIcon icon={faTrash} />
                    </button>
                    <button
                      onClick={() => handleEdit(product.id)}
                      className="edit-button"
                    >
                      <FontAwesomeIcon icon={faPenToSquare} />
                    </button>
                  </div>
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
