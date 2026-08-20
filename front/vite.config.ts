import { defineConfig, loadEnv } from 'vite'
import react from '@vitejs/plugin-react'

// https://vite.dev/config/
export default defineConfig(({ mode }) => {
    const hostname = process.env.VITE_SITE_URL || 'localhost';
    console.log(hostname);
    const env = loadEnv(mode, process.cwd(), '')
    return {
        server: {
            host: true,
            allowedHosts: ['avelee.online']
        },
        plugins: [react()],
    }
})
