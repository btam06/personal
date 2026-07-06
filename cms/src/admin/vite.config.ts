import { mergeConfig, type UserConfig } from 'vite';

export default (config: UserConfig) => {
  // Important: always return the modified config
  const hostname = process.env.SITE_URL || 'localhost';

  return mergeConfig(config, {
    server: {
      allowedHosts: [hostname],
    },
    resolve: {
      alias: {
        '@': '/src',
      },
    },
  });
};
