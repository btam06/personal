import './App.css'

import { BrowserRouter, Routes, Route } from 'react-router'
import Layout from './layout/main'
import ViewPage from './pages/view-page'

function App() {
  return (
    <BrowserRouter>
        <Routes>
            <Route element={<Layout />}>
                <Route path="*" element={<ViewPage />} />
            </Route>
        </Routes>
    </BrowserRouter>
  )
}

export default App
