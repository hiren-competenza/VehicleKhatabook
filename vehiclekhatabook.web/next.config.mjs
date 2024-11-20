// /** @type {import('next').NextConfig} */
// const nextConfig = {
//     reactStrictMode: false,
//     images: {
//       //! remove this property if will get api from the server
//       unoptimized: true,
//       domains: ['www.keyweo.com'],
//     },
//   };

// export default nextConfig;

// next.config.mjs
export default {
  reactStrictMode: false,

  images: {
    remotePatterns: [
      {
        protocol: 'http',
        hostname: '**',
      },
      {
        protocol: 'https',
        hostname: '**',
      },
    ],
 
  },
};