import './App.css'

import { BrowserRouter, Routes, Route } from 'react-router'
import Layout from './layout/main'
import NotFound from './pages/not-found'
import Content from './pages/content'

function App() {
  return (
    <BrowserRouter>
        <Routes>
            <Route element={<Layout />}>
                <Route index path="/:page" element={<Content />} />
                <Route path="*" element={<NotFound />} />
            </Route>
        </Routes>
    </BrowserRouter>
  )
}

export default App
