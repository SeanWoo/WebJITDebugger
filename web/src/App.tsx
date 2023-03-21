import React, { useState } from 'react';
import Editor from 'react-simple-code-editor';
import { highlight, languages } from 'prismjs/components/prism-core';
import 'prismjs/components/prism-clike';
import 'prismjs/components/prism-csharp';
import 'prismjs/components/prism-javascript';
import 'prismjs/themes/prism.css'; //Example style, you can use another
import { Container } from './styles'
import axios from 'axios';
import { Button, Upload } from 'antd';
import { UploadOutlined } from '@ant-design/icons'

const INITIAL_CODE = 
`public static class Program {
    public static void Main(string[] args) {
        Console.WriteLine("Hello, World!");
    }
}
`

const INITIAL_RESULT = "Нажмите кнопку 'Сохранить' для получения результата"

function App() {
  const [code, setCode] = useState(INITIAL_CODE);
  const [result, setResult] = useState(INITIAL_RESULT);

  const handleSubmitCode = () => {
    axios.post("/api/disassembler", {
      "code": code
    })
      .then(value => setResult(value.data.assemblyCode))
      .catch(value => {
        console.log(value)
        if (value.response.status !== 400) {
          setResult(value.message)
          return;
        }
        
        const response = value.response.data
        setResult(`Error type: ${response.type}\nMessage: ${response.message}`)
      });
  }

  return (
    <div>
      <Button onClick={handleSubmitCode}>Отправить</Button>
      <Upload action="https://www.mocky.io/v2/5cc8019d300000980a055e76" directory>
        <Button icon={<UploadOutlined />}>Upload Directory</Button>
      </Upload>
      <Container>
        <Editor
          value={code}
          onValueChange={code => setCode(code)}
          highlight={code => highlight(code, languages.csharp)}
          padding={10}
          style={{
            fontFamily: '"Fira code", "Fira Mono", monospace',
            fontSize: 12,
            width: "100%",
          }}
        />
        <Editor
          value={result}
          onValueChange={() => undefined}
          highlight={code => highlight(code, languages.text)}
          padding={10}
          style={{
            fontFamily: '"Fira code", "Fira Mono", monospace',
            fontSize: 12,
            width: "100%",
          }}
        />
      </Container>
    </div>
  );
}

export default App;
