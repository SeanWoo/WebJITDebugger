declare module 'prismjs/components/prism-core' {
    const highlight: (code: string, style: any) => any;
    const languages: Record<string, any>;
    export { highlight, languages };
  }