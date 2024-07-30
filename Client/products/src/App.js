import React from "react";
import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import Product from "./Components/Product";
import AddProduct from "./Components/AddProduct";

function App() {
  return (
    <Router>
      <div>
        <Routes>
          <Route path="/" element={<Product />} />
          <Route path="/add-product" element={<AddProduct />} />
        </Routes>
      </div>
    </Router>
  );
}

export default App;
