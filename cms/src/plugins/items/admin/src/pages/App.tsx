import { Page } from "@strapi/strapi/admin";
import { Routes, Route } from "react-router-dom";

import { List } from "./List";

const App = () => {
  return (
    <Routes>
      <Route index element={<List />} />
      <Route path="*" element={<Page.Error />} />
    </Routes>
  );
};

export default App;
