import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Product from "./Components/Product";
import AddProduct from "./Components/AddProduct";
import UpdateProductPage from "./Components/UpdateProductPage";

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<Product />} />
          <Route path="/add-product" element={<AddProduct />} />
          <Route path="/update-product/:id" element={<UpdateProductPage />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
