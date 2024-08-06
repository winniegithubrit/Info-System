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
  const [students, setStudents] = useState([]);
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
      console.log("Refreshing product data:", new Date());
    } catch (error) {
      console.error("Error fetching products:", error);
    }
  };

  // Fetch students function
  const fetchStudents = async () => {
    try {
      const response = await fetch(
        "https://localhost:5001/api/Student/students"
      );
      const data = await response.json();
      // Sorting students in descending order based on id
      const sortedData = data.sort((a, b) => b.id - a.id);
      setStudents(sortedData);
      console.log("Refreshing students data:", new Date());
    } catch (error) {
      console.error("Error fetching students:", error);
    }
  };

  // Fetch products every 5 seconds
  useEffect(() => {
    fetchProducts();
    const interval = setInterval(fetchProducts, 5000);
    return () => clearInterval(interval);
  }, []);

  // Fetch students only once when the component mounts
  useEffect(() => {
    fetchStudents();
    const intervals = setInterval(fetchStudents, 10000);
    return () => clearInterval(intervals)

  }, []);

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

      <div className="table-wrapper">
        <div className="product-table">
          <h2>Products</h2>
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

        <div className="student-table">
          <h2>Student List</h2>
          <table>
            <thead>
              <tr>
                {/* <th>Id</th> */}
                <th>First Name</th>
                <th>Last Name</th>
                <th>age</th>
                <th>grade</th>
                
              </tr>
            </thead>
            <tbody>
              {students.map((student) => (
                <tr key={student.id}>
                  {/* <td>{student.id}</td> */}
                  <td>{student.firstName}</td>
                  <td>{student.lastName}</td>
                  <td>{student.age}</td>
                  <td>{student.grade}</td>
                
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default Product;
